#set(CMAKE_CXX_SCAN_FOR_MODULES OFF) # Uncomment for faster build if you don't use C++20 modules

include(${CMAKE_CURRENT_LIST_DIR}/compiler_options.cmake)
include(${CMAKE_CURRENT_LIST_DIR}/compiler_definitions.cmake)
include(${CMAKE_CURRENT_LIST_DIR}/functions.cmake)

enable_testing()
