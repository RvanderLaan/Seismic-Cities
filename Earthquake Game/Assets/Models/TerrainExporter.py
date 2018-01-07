# Separates a mesh into separate objects based on their material and UV unwraps them
# Make sure to select the object first!

bl_info = {
    "name": "Seismic Cities Terrain Exporter",
    "category": "Object",
    "author": "Remi van der Laan"
}

import bpy
import bmesh

class TerrainExporter(bpy.types.Operator):
    bl_label = "SS Terain Exporter"
    bl_idname = "object.terrain_exporter"
    bl_options = {'REGISTER', 'UNDO'}
    
    def execute(self, context):

        # Switch to 3d viewport
        area = bpy.context.area
        old_type = area.type
        area.type = 'VIEW_3D'
        
        # Get selected object
        obj = bpy.context.active_object
        
        # Parent for separate objects
        empty = bpy.data.objects.new("Separated", None)
        bpy.context.scene.objects.link(empty)
        bpy.context.scene.objects.active = obj

        # Duplicate selected object
        dupe = bpy.ops.object.duplicate()
        obj.hide = True # Hide original object
        
        # Apply modifiers
        for modifier in obj.modifiers:
            bpy.ops.object.modifier_apply(modifier=modifier.name)

        bpy.ops.object.mode_set(mode = 'EDIT')
        
        # Apply modifiers
        for modifier in obj.modifiers:
            bpy.ops.object.modifier_apply(modifier=modifier.name)

        # UV unwrap from front
        bpy.ops.view3d.viewnumpad(type='FRONT') # Front view
        bpy.ops.mesh.select_all(action = 'SELECT')
        
        bpy.ops.uv.project_from_view(orthographic=True, camera_bounds=False, correct_aspect=True, clip_to_bounds=False, scale_to_bounds=False)

        # Separate layers based on material
        bpy.ops.mesh.separate(type = 'MATERIAL')
        bpy.ops.mesh.select_all(action = 'DESELECT')
        bpy.ops.object.mode_set(mode = 'OBJECT')
        
        # Seperate loose objects and simplify mesh
        bpy.ops.object.select_all(action='SELECT')
        for obj in bpy.context.selected_objects:
            bpy.context.scene.objects.active = obj
            
            if bpy.context.active_object.active_material is not None:
                bpy.ops.object.mode_set(mode = 'EDIT')
                
                # Dissolve faces
                bpy.ops.mesh.select_all(action='SELECT')
                bpy.ops.mesh.dissolve_faces()
                
                # Triangluate
                me = obj.data
                bm = bmesh.from_edit_mesh(me)

                bmesh.ops.triangulate(bm, faces=bm.faces[:], quad_method=0, ngon_method=0)
                # Show the updates in the viewport and recalculate n-gon tessellation.
                bmesh.update_edit_mesh(me, True)
                
                # Seperate layers by loose parts
                bpy.ops.mesh.separate(type='LOOSE')
                bpy.ops.mesh.select_all(action = 'DESELECT')
                bpy.ops.object.mode_set(mode = 'OBJECT')

        # Rename layers based on material name
        bpy.ops.object.select_all(action='SELECT')
        for obj in bpy.context.selected_objects:
            bpy.context.scene.objects.active = obj
            if bpy.context.active_object.active_material is not None:
                obj.name = bpy.context.active_object.active_material.name
                obj.parent = empty

        # Return to script
        area.type = old_type
        
        return {'FINISHED'}

def register():
    bpy.utils.register_class(TerrainExporter)


def unregister():
    bpy.utils.unregister_class(TerrainExporter)

if __name__ == "__main__":
    register()
