import bpy
import sys
import os

def import_svg(svg_filepath):
    # Import SVG
    bpy.ops.import_curve.svg(filepath=svg_filepath)

def convert_svg_to_mesh_and_extrude(extrude_depth):
    # Convert imported SVG to mesh and apply extrusion
    bpy.ops.object.select_all(action='DESELECT')
    imported_objects = []
    for obj in bpy.context.scene.objects:
        if obj.type == 'CURVE':
            obj.select_set(True)
            bpy.context.view_layer.objects.active = obj
            # Set extrude depth
            obj.data.extrude = extrude_depth
            # Convert to mesh
            bpy.ops.object.convert(target='MESH')
            imported_objects.append(obj)
    return imported_objects

def remove_default_objects():
    # Remove default objects (camera, light, cube)
    bpy.ops.object.select_all(action='DESELECT')
    bpy.data.objects.remove(bpy.data.objects.get('Camera'), do_unlink=True)
    bpy.data.objects.remove(bpy.data.objects.get('Light'), do_unlink=True)
    bpy.data.objects.remove(bpy.data.objects.get('Cube'), do_unlink=True)

def scale_up_objects(scale_factor):
    # Scale up imported objects
    for obj in bpy.context.scene.objects:
        obj.scale.x *= scale_factor
        obj.scale.y *= scale_factor


def export_to_fbx(fbx_filepath):
    # Export to FBX
    bpy.ops.export_scene.fbx(filepath=fbx_filepath)

def main(svg_filepath, fbx_filepath, extrude_depth, scale_factor):
    import_svg(svg_filepath)
    imported_objects = convert_svg_to_mesh_and_extrude(extrude_depth)
    if not imported_objects:
        print("No objects were imported from the SVG.")
        return
    remove_default_objects()
    scale_up_objects(scale_factor)
    # Select only the imported and converted objects
    bpy.ops.object.select_all(action='DESELECT')
    for obj in imported_objects:
        obj.select_set(True)
    export_to_fbx(fbx_filepath)

if __name__ == "__main__":
    # Expecting arguments: svg_filepath, fbx_filepath, extrude_depth, scale_factor
    if len(sys.argv) < 7:
        print("Usage: blender --background --python script.py -- <svg_filepath> <fbx_filepath> <extrude_depth> <scale_factor>")
    else:
        svg_filepath = sys.argv[-4]
        fbx_filepath = sys.argv[-3]
        extrude_depth = float(sys.argv[-2])
        scale_factor = float(sys.argv[-1])
        
        # Check if the SVG file exists
        if not os.path.isfile(svg_filepath):
            print(f"SVG file '{svg_filepath}' not found.")
            sys.exit(1)
        
        print(f"Importing SVG file: {svg_filepath}")
        main(svg_filepath, fbx_filepath, extrude_depth, scale_factor)
        print(f"Exported FBX file to: {fbx_filepath}")
