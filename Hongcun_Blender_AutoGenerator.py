"""
Hongcun_Blender_AutoGenerator.py
================================================================================
An industrial-grade, production-ready 3D automated scene generator script for 
Blender 4.5.11 (and backward compatible with 3.x and 4.x).

This script automatically clears the scene, configures materials with advanced 
fallback handling, generates the historical Yongle Era (Era 1) Ox-shaped 
water network of Hongcun, models traditional Huizhou-style residences with 
layered horse-head walls (马头墙), computes 3D pipelines matching high-to-low 
elevations, and integrates a fully-functional rigid body physics simulation.

Usage:
    1. Open Blender 4.5.11.
    2. Switch to the 'Scripting' workspace.
    3. Click 'New', paste this script, and click 'Run Script' (Play button).
================================================================================
"""

import bpy
import math
import mathutils

# ==============================================================================
# 1. SCENE CLEAN & INITIALIZATION (安全清空场景)
# ==============================================================================
def clear_scene():
    """
    Directly removes all scene objects from data, bypassing operator poll/context
    limitations and ensuring a perfectly clean canvas.
    """
    print("[Hongcun Generator] Cleaning scene objects...")
    # Switch to object mode if not already
    if bpy.ops.object.mode_set.poll():
        try:
            bpy.ops.object.mode_set(mode='OBJECT')
        except Exception:
            pass
            
    # Unlink and delete all meshes, curves, cameras, lights, etc.
    for obj in list(bpy.data.objects):
        bpy.data.objects.remove(obj, do_unlink=True)
        
    # Purge unused mesh blocks from memory
    for mesh in list(bpy.data.meshes):
        bpy.data.meshes.remove(mesh)
        
    # Purge unused materials
    for mat in list(bpy.data.materials):
        bpy.data.materials.remove(mat)

# ==============================================================================
# 2. DEFINING THE GEOMETRICAL NODES & EDGES (定义永乐水网拓扑)
# ==============================================================================
# Node definitions from historic Hongcun Yongle Era database (including South Lake)
# Coordinates map 2D SVG canvas (740x500) to 3D Blender space.
# Vertical elevation is scaled by 0.1 to perfectly match Z-slope (e.g. 120 -> Z=12.0)
ERA1_NODES = [
    {"name": "XiXi_River", "type": "RiverSource", "x": 80, "y": 150, "elevation": 120.0},
    {"name": "Ancient_Stage", "type": "Residential", "x": 120, "y": 320, "elevation": 110.0},
    {"name": "Shijie_Sluice", "type": "Gate", "x": 220, "y": 100, "elevation": 115.0},
    {"name": "Wang_Clan_Hall", "type": "Residential", "x": 380, "y": 160, "elevation": 112.0},
    {"name": "Moon_Pond", "type": "MoonPond", "x": 520, "y": 250, "elevation": 106.0},
    {"name": "Lexu_Hall", "type": "Residential", "x": 440, "y": 340, "elevation": 105.0},
    {"name": "South_Lake", "type": "SouthLake", "x": 300, "y": 440, "elevation": 98.0},
]

# Canals / Pipelines (Edges) connecting the nodes representing the physical water path
ERA1_EDGES = [
    ("XiXi_River", "Ancient_Stage"),
    ("XiXi_River", "Shijie_Sluice"),
    ("Shijie_Sluice", "Wang_Clan_Hall"),
    ("Wang_Clan_Hall", "Moon_Pond"),
    ("Moon_Pond", "Lexu_Hall"),
    ("Lexu_Hall", "South_Lake"),
]

def get_3d_coords(x, y, elevation):
    """
    Translates 2D coordinates and elevations to 3D Blender coordinates.
    X -> maps to X (horizontal grid scaling)
    Y -> maps to Y (depth grid scaling)
    Elevation * 0.1 -> maps to Z (upwards elevation slope)
    """
    pos_x = (x - 370) * 0.12
    pos_y = (y - 250) * 0.12
    pos_z = elevation * 0.1
    return (pos_x, pos_y, pos_z)

# Cache 3D coordinates for fast edge lookup
NODE_3D_COORDS = {
    node["name"]: get_3d_coords(node["x"], node["y"], node["elevation"])
    for node in ERA1_NODES
}

# ==============================================================================
# 3. ink-WASH MATERIAL GENERATOR (水墨风格材质配置)
# ==============================================================================
def set_shader_input(node, input_name, value):
    """
    Safely sets shader input values with robust version checks to ensure
    seamless compatibility between Blender 3.x and 4.x/5.x.
    """
    if input_name in node.inputs:
        node.inputs[input_name].default_value = value
    elif input_name == "Transmission Weight" and "Transmission" in node.inputs:
        node.inputs["Transmission"].default_value = value
    elif input_name == "Transmission" and "Transmission Weight" in node.inputs:
        node.inputs["Transmission Weight"].default_value = value

def create_ink_material(name, base_color, roughness, transmission=None):
    """
    Creates a highly compatible material utilizing the modern Principled BSDF shader.
    """
    mat = bpy.data.materials.get(name)
    if mat is None:
        mat = bpy.data.materials.new(name=name)
        
    mat.use_nodes = True
    nodes = mat.node_tree.nodes
    nodes.clear()
    
    # Create BSDF and output nodes
    node_bsdf = nodes.new(type='ShaderNodeBsdfPrincipled')
    node_output = nodes.new(type='ShaderNodeOutputMaterial')
    
    # Link them
    links = mat.node_tree.links
    links.new(node_bsdf.outputs['BSDF'], node_output.inputs['Surface'])
    
    # Configure surface parameters
    set_shader_input(node_bsdf, "Base Color", base_color)
    set_shader_input(node_bsdf, "Roughness", roughness)
    
    if transmission is not None:
        set_shader_input(node_bsdf, "Transmission Weight", transmission)
        # Apply alpha blending for real-time glass/fluid rendering in Eevee/Viewport
        try:
            mat.blend_method = 'BLEND'
            mat.shadow_method = 'NONE'
        except AttributeError:
            pass
            
    return mat

# Declare global materials to be assigned across meshes
white_wall_mat = None
black_tile_mat = None
jade_water_mat = None

def init_all_materials():
    global white_wall_mat, black_tile_mat, jade_water_mat
    print("[Hongcun Generator] Creating ink-wash themed materials...")
    # White Wall: Pure Xuan paper look (#ffffff, Roughness=0.9)
    white_wall_mat = create_ink_material("White_Wall", (1.0, 1.0, 1.0, 1.0), 0.9)
    # Black Tile: Dense charcoal paint (#111111, Roughness=0.8)
    black_tile_mat = create_ink_material("Black_Tile", (0.067, 0.067, 0.067, 1.0), 0.8)
    # Jade Water: Deep emerald transparent stream (#2a7f6f, Transmission=0.9, Roughness=0.05)
    jade_water_mat = create_ink_material("Jade_Water", (0.165, 0.498, 0.435, 1.0), 0.05, transmission=0.9)

# ==============================================================================
# 4. PROCEDURAL RESIDENCE BUILDER (马头墙民居生成器)
# ==============================================================================
def build_huizhou_residence(name, pos_x, pos_y, pos_z):
    """
    Builds a classic multi-material Huizhou residence using primitive joining
    to form a single solid mesh without risking vertex extrusion bugs.
    """
    print(f"[Hongcun Generator] Modeling residence: {name}...")
    parts = []
    
    # 1. White Main Body (x=6, y=6, z=5)
    bpy.ops.mesh.primitive_cube_add(size=1.0)
    body = bpy.context.active_object
    body.name = f"{name}_Body"
    body.dimensions = (6.0, 6.0, 5.0)
    body.location = (pos_x, pos_y, pos_z + 2.5)
    body.data.materials.append(white_wall_mat)
    parts.append(body)
    
    # 2. Black Pitch Roof (x=6.4, y=6.4, z=1.2)
    bpy.ops.mesh.primitive_cube_add(size=1.0)
    roof = bpy.context.active_object
    roof.name = f"{name}_Roof"
    roof.dimensions = (6.4, 6.4, 1.2)
    roof.location = (pos_x, pos_y, pos_z + 5.0 + 0.6)
    roof.data.materials.append(black_tile_mat)
    parts.append(roof)
    
    # 3. Triple Horse-head Walls (三级叠落马头墙)
    # Placed symmetrically at the left and right X-axis boundaries (X_offset = -3.05 and +3.05)
    # Stepping gracefully down on the Y-axis from back (high) to front (low)
    for x_sign in [-1, 1]:
        x_offset = x_sign * 3.05
        
        # Step 1 (High Step - placed at Y = 1.8)
        bpy.ops.mesh.primitive_cube_add(size=1.0)
        s1 = bpy.context.active_object
        s1.name = f"{name}_Step_High"
        s1.dimensions = (0.2, 1.8, 2.0)
        s1.location = (pos_x + x_offset, pos_y + 1.8, pos_z + 5.0 + 1.0)
        s1.data.materials.append(black_tile_mat)
        parts.append(s1)
        
        # Step 2 (Medium Step - placed at Y = 0.0)
        bpy.ops.mesh.primitive_cube_add(size=1.0)
        s2 = bpy.context.active_object
        s2.name = f"{name}_Step_Med"
        s2.dimensions = (0.2, 1.8, 1.4)
        s2.location = (pos_x + x_offset, pos_y + 0.0, pos_z + 5.0 + 0.7)
        s2.data.materials.append(black_tile_mat)
        parts.append(s2)
        
        # Step 3 (Low Step - placed at Y = -1.8)
        bpy.ops.mesh.primitive_cube_add(size=1.0)
        s3 = bpy.context.active_object
        s3.name = f"{name}_Step_Low"
        s3.dimensions = (0.2, 1.8, 0.8)
        s3.location = (pos_x + x_offset, pos_y - 1.8, pos_z + 5.0 + 0.4)
        s3.data.materials.append(black_tile_mat)
        parts.append(s3)

    # 4. Select and Merge all components into the main white body
    bpy.ops.object.select_all(action='DESELECT')
    for part in parts:
        part.select_set(True)
    body.select_set(True)
    
    # Make body the active component to retain its pivot and name
    bpy.context.view_layer.objects.active = body
    bpy.ops.object.join()
    
    # Finalize naming
    final_building = bpy.context.active_object
    final_building.name = name
    return final_building

# ==============================================================================
# 5. WATER CHANNELS, MOON POND & SOUTH LAKE (高程差水系与立体水管)
# ==============================================================================
def build_moon_pond(name, pos_x, pos_y, pos_z):
    """
    Creates a perfect semi-circular pool representing the Moon Pond (月沼 - 牛胃),
    using an automated clean boolean modifier cut.
    """
    print(f"[Hongcun Generator] Generating Moon Pond: {name}...")
    # Base cylinder
    bpy.ops.mesh.primitive_cylinder_add(radius=6.0, depth=0.8, vertices=64, location=(pos_x, pos_y, pos_z))
    pond = bpy.context.active_object
    pond.name = name
    pond.data.materials.append(jade_water_mat)
    
    # Helper cutter cube placed on the negative X half
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=(pos_x - 3.0, pos_y, pos_z))
    cutter = bpy.context.active_object
    cutter.name = "MoonPond_Cutter"
    cutter.dimensions = (6.0, 13.0, 2.0)
    
    # Set active and apply boolean modifier
    pond_modifier = pond.modifiers.new(name="Pond_Cut", type='BOOLEAN')
    pond_modifier.operation = 'DIFFERENCE'
    pond_modifier.object = cutter
    
    bpy.context.view_layer.objects.active = pond
    bpy.ops.object.modifier_apply(modifier="Pond_Cut")
    
    # Wipe out the cutter helper object
    bpy.data.objects.remove(cutter, do_unlink=True)
    return pond

def build_south_lake(name, pos_x, pos_y, pos_z):
    """
    Creates a large, shallow circular pool at the lowest elevation to represent
    the magnificent South Lake (南湖 - 牛肚).
    """
    print(f"[Hongcun Generator] Generating South Lake: {name}...")
    bpy.ops.mesh.primitive_cylinder_add(radius=10.0, depth=0.6, vertices=64, location=(pos_x, pos_y, pos_z))
    lake = bpy.context.active_object
    lake.name = name
    lake.data.materials.append(jade_water_mat)
    return lake

def build_gate_house(name, pos_x, pos_y, pos_z):
    """
    Generates the Sluice Gate (石碣水口) as a unique archway-style gateway node.
    """
    print(f"[Hongcun Generator] Generating Sluice Gate: {name}...")
    # Left pillar
    bpy.ops.mesh.primitive_cube_add(size=1.0)
    lp = bpy.context.active_object
    lp.name = "Gate_Left"
    lp.dimensions = (0.6, 0.6, 3.5)
    lp.location = (pos_x - 1.2, pos_y, pos_z + 1.75)
    lp.data.materials.append(black_tile_mat)
    
    # Right pillar
    bpy.ops.mesh.primitive_cube_add(size=1.0)
    rp = bpy.context.active_object
    rp.name = "Gate_Right"
    rp.dimensions = (0.6, 0.6, 3.5)
    rp.location = (pos_x + 1.2, pos_y, pos_z + 1.75)
    rp.data.materials.append(black_tile_mat)
    
    # Sluice wooden gate board
    bpy.ops.mesh.primitive_cube_add(size=1.0)
    board = bpy.context.active_object
    board.name = "Gate_Board"
    board.dimensions = (2.0, 0.2, 2.5)
    board.location = (pos_x, pos_y, pos_z + 1.25)
    board.data.materials.append(white_wall_mat)

    # Merge them
    bpy.ops.object.select_all(action='DESELECT')
    lp.select_set(True)
    rp.select_set(True)
    board.select_set(True)
    bpy.context.view_layer.objects.active = board
    bpy.ops.object.join()
    
    final_gate = bpy.context.active_object
    final_gate.name = name
    return final_gate

def build_source_spring(name, pos_x, pos_y, pos_z):
    """
    Generates the River Source (西溪源头) as a layered slate mountain rock formation.
    """
    print(f"[Hongcun Generator] Generating River Source: {name}...")
    parts = []
    for i in range(5):
        size = 3.5 - i * 0.6
        bpy.ops.mesh.primitive_ico_sphere_add(subdivisions=2, radius=size, location=(pos_x, pos_y + (i * 0.4), pos_z + (i * 0.5)))
        rock = bpy.context.active_object
        rock.name = f"Spring_Rock_{i}"
        rock.data.materials.append(black_tile_mat)
        parts.append(rock)
        
    bpy.ops.object.select_all(action='DESELECT')
    for part in parts:
        part.select_set(True)
    bpy.context.view_layer.objects.active = parts[0]
    bpy.ops.object.join()
    
    final_spring = bpy.context.active_object
    final_spring.name = name
    return final_spring

def build_elevation_pipe(from_name, to_name, p1_coords, p2_coords):
    """
    Advanced mathematical calculation to rotate and translate a 3D cylinder so 
    it aligns perfectly with the 3D elevation slope between two nodes.
    """
    p1 = mathutils.Vector(p1_coords)
    p2 = mathutils.Vector(p2_coords)
    vector = p2 - p1
    length = vector.length
    midpoint = (p1 + p2) / 2.0
    
    # Calculate quaternion rotation from Z-axis vector (0, 0, 1) to pointing vector direction
    direction = vector.normalized()
    z_axis = mathutils.Vector((0, 0, 1))
    rotation_quat = z_axis.rotation_difference(direction)
    
    # Instantiate cylinder
    bpy.ops.mesh.primitive_cylinder_add(radius=0.45, depth=length, vertices=24, location=midpoint)
    pipe = bpy.context.active_object
    pipe.name = f"Pipe_{from_name}_to_{to_name}"
    
    # Align orientation with mathematical rotation difference
    pipe.rotation_mode = 'QUATERNION'
    pipe.rotation_quaternion = rotation_quat
    pipe.data.materials.append(jade_water_mat)
    return pipe

# ==============================================================================
# 6. RIGID BODY ENGINE INTEGRATION (物理引擎与测试木柴)
# ==============================================================================
def apply_rigid_body(obj, body_type='PASSIVE', shape='MESH', mass=1.0):
    """
    Hooks an object up with the built-in Blender rigid body physics engine.
    Passive objects behave as precise static mesh collision obstacles.
    """
    bpy.ops.object.select_all(action='DESELECT')
    obj.select_set(True)
    bpy.context.view_layer.objects.active = obj
    
    # Add to rigid body database if not already bound
    if not obj.rigid_body:
        bpy.ops.rigidbody.object_add()
        
    rb = obj.rigid_body
    rb.type = body_type
    rb.collision_shape = shape
    rb.mass = mass
    # Set high friction to prevent wood sliding off too fast
    rb.friction = 0.8
    rb.restitution = 0.1

def add_ground_plate():
    """
    Adds a solid ground plane to represent the paper canvas (宣纸大地) 
    and act as a final safety physical container.
    """
    print("[Hongcun Generator] Spawning solid ground plate...")
    bpy.ops.mesh.primitive_plane_add(size=120.0, location=(0, 0, 0))
    ground = bpy.context.active_object
    ground.name = "Ink_Paper_Ground"
    ground.data.materials.append(white_wall_mat)
    apply_rigid_body(ground, body_type='PASSIVE', shape='MESH')
    return ground

def add_test_log():
    """
    Generates a Test Log (测试木柴) perfectly positioned at height Z=15 
    directly above the XiXi River source to test physical flow.
    """
    print("[Hongcun Generator] Spawning dynamic test log (Test_Log)...")
    source_coords = NODE_3D_COORDS["XiXi_River"]
    
    # Spawn a 1x1x1 test cube at height Z=15
    bpy.ops.mesh.primitive_cube_add(size=1.0, location=(source_coords[0], source_coords[1], 15.0))
    log = bpy.context.active_object
    log.name = "Test_Log"
    
    # Style it with a custom rustic timber look
    wood_mat = create_ink_material("Rustic_Timber", (0.52, 0.31, 0.15, 1.0), 0.85)
    log.data.materials.append(wood_mat)
    
    # Hook up dynamic rigid body
    apply_rigid_body(log, body_type='ACTIVE', shape='BOX', mass=1.0)
    return log

# ==============================================================================
# 7. EXECUTION ORCHESTRATOR (场景自动化拼装流程)
# ==============================================================================
def main():
    print("==========================================================")
    print("  [HONGCONG BLENDER AUTO GENERATOR VERSION 4.5.11] STARTED ")
    print("==========================================================")
    
    # Step 1: Clean Up Scene
    clear_scene()
    
    # Step 2: Initialize Shader Materials
    init_all_materials()
    
    # Step 3: Add Physical Ground Plate
    ground = add_ground_plate()
    
    # Step 4: Build Scenic Nodes
    scenic_objects = []
    for node in ERA1_NODES:
        coords = NODE_3D_COORDS[node["name"]]
        obj = None
        
        if node["type"] == "Residential":
            obj = build_huizhou_residence(node["name"], coords[0], coords[1], coords[2])
        elif node["type"] == "MoonPond":
            obj = build_moon_pond(node["name"], coords[0], coords[1], coords[2])
        elif node["type"] == "SouthLake":
            obj = build_south_lake(node["name"], coords[0], coords[1], coords[2])
        elif node["type"] == "Gate":
            obj = build_gate_house(node["name"], coords[0], coords[1], coords[2])
        elif node["type"] == "RiverSource":
            obj = build_source_spring(node["name"], coords[0], coords[1], coords[2])
            
        if obj:
            scenic_objects.append(obj)
            
    # Step 5: Build High-to-Low Elevation Canal Pipelines
    pipe_objects = []
    for from_node, to_node in ERA1_EDGES:
        if from_node in NODE_3D_COORDS and to_node in NODE_3D_COORDS:
            p1 = NODE_3D_COORDS[from_node]
            p2 = NODE_3D_COORDS[to_node]
            pipe = build_elevation_pipe(from_node, to_node, p1, p2)
            pipe_objects.append(pipe)
            
    # Step 6: Apply Static Mesh Rigid Colliders to avoid logs falling through
    print("[Hongcun Generator] Initializing passive collision rigid bodies...")
    for obj in scenic_objects + pipe_objects:
        apply_rigid_body(obj, body_type='PASSIVE', shape='MESH')
        
    # Step 7: Create Dynamic Active Rigid Body wood piece to test the scene
    add_test_log()
    
    # Step 8: Focus and Frame View (Slightly adjust Viewport to frame the models)
    bpy.ops.object.select_all(action='DESELECT')
    for obj in scenic_objects:
        obj.select_set(True)
    
    # View port auto framing (if interactive session)
    try:
        for area in bpy.context.screen.areas:
            if area.type == 'VIEW_3D':
                for region in area.regions:
                    if region.type == 'WINDOW':
                        override = {'area': area, 'region': region, 'edit_object': bpy.context.edit_object}
                        bpy.ops.view3d.view_selected(override)
                        break
    except Exception:
        pass
        
    print("==========================================================")
    print("  [HONGCONG BLENDER AUTO GENERATOR] SUCCESSFULLY COMPILED! ")
    print("==========================================================")

if __name__ == "__main__":
    main()
