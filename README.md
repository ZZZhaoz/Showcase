# The Earth–Moon–Sun Dance  
**CSC317 Fall 2025 – Showcase Extra Credit**

---

## Personal Information  
**Full Name:** Zihan Zhao  
**UtorID:** zhaoz188  
**Student Number:** 1010128274  
**Assignment Augmented:** Assignment 6 (Shader Pipeline)

---

## Instructions

### 0. Clone the Repository (with Submodules)

To get started, clone this repository **including all its submodule dependencies**:

`git clone --recursive https://github.com/ZZZhaoz/showcase.git`

### 1. Configure the Project (CMake)

From the Code/ directory:
`
mkdir build-release 
`
`
cd build-release
`
`
cmake ../ -DCMAKE_BUILD_TYPE=Release
`

### 2. Build the Project
#### Windows (Visual Studio 2022)


Open: Code/build-release/shaderpipeline.sln

Then build the project. Ensure the output directory of shaderpipeline is set to build-release/
<img width="2489" height="1442" alt="屏幕截图 2025-11-26 013159" src="https://github.com/user-attachments/assets/3bee6aab-69c4-4508-9c39-2f1d807f7368" />

#### macOS / Linux (CMake + Make/Ninja)
From `Code/build-release/`:

```bash
cmake --build . --parallel
```

### 3. Run the Program
Executable will appear in build-release/:

./shaderpipeline       (Linux/macOS)
shaderpipeline.exe     (Windows)

---

## Description
This Showcase enhances Assignment 6 with a complete real-time, procedural Earth–Moon–Sun system, integrating multiple shader modules and animation logic.

### 1. main.cpp – Animation, Camera, and Shader Integration
Real-time orbital mechanics for Earth and Moon

“Follow Earth” camera with custom lookAt implementation

Keyboard controls:

- A: toggle orbit animation

- L: locked camera mode

- H: reset

- Z: zoom

- R: reload shaders

Passed real Sun position (sun_world_pos) into shaders for lighting

Shader hot-reloading

### 2. planet.fs – Full Procedural Planet Renderer
Procedural Sun with animated convection (Perlin noise)

Earth layers:

oceans, coasts, land, mountains, deserts, snow, polar caps

procedural clouds

cloud shadowing

night-time city lights

Bump-mapped Moon terrain

Fully procedural multi-octave starfield background

Unified physically-inspired lighting model

### 3. model.glsl – Orbital Transform System
Transformation hierarchy:

Sun

Earth orbit

Earth spin

Moon orbit

Moon tilt

Moon scale

Starfield sphere transform

Tuned orbital parameters for realism

### 4. bump_height.glsl – Terrain / Height Modeling
Multi-layer Earth height: oceans → coasts → land → mountains → snow

Procedural lunar crater approximation

Height drives shading, cloud shadows, biome transitions

---

## Acknowledgements
CSC317 Assignment 6 starter code

libigl (icosahedron mesh utilities)

GLFW (window / input)

Eigen (math library)

Ken Perlin (noise reference)

GLAD (OpenGL loader)
