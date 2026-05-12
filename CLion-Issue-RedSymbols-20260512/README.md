Instructions to reproduce the issue with red symbols in Clion:

1. Open the folder in Clion.
2. Set the `VCPKG_ROOT` environment variable to the path of the vcpkg installation or modify the vcpkg installation path in the `CMakePresets.json` file.
3. Disable the default `Debug` CMake configuration. Enable `Windows x64 (Debug)` configuraton instead.
4. Configure the CMake project.
5. Open the `my_app/main.cpp` file.
6. Observe the red symbols in the file. 
