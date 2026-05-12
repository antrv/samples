function(get_source_files source_dir output_variable)
    file(GLOB_RECURSE _source_files CONFIGURE_DEPENDS
        "${source_dir}/*.cpp"
        "${source_dir}/*.cxx"
        "${source_dir}/*.h"
        "${source_dir}/*.hpp"
        "${source_dir}/*.inl"
        "${source_dir}/*.ixx"
    )

    source_group(TREE "${source_dir}" FILES ${_source_files})
    set(${output_variable} ${_source_files} PARENT_SCOPE)
endfunction()

function(print_cmake_variables)
    get_cmake_property(_variable_names VARIABLES)
    list (SORT _variable_names)
    foreach (_variable_name ${_variable_names})
        message(STATUS "${_variable_name}=${${_variable_name}}")
    endforeach()
endfunction()

function(configure_file_on_build)
    cmake_parse_arguments(
        PARSE_ARGV 0 arg
        "" # keywords that have no value following them
        "TARGET;INPUT_FILE;OUTPUT_FILE" # keywords for this function or macro which are followed by one value
        "INCLUDES" # keywords for this function or macro which can be followed by more than one value
    )

    if("${arg_TARGET}" STREQUAL "")
        message(FATAL_ERROR "TARGET is not set in configure_file_on_build")
    endif()

    if("${arg_INPUT_FILE}" STREQUAL "")
        message(FATAL_ERROR "INPUT_FILE is not set in configure_file_on_build")
    endif()

    if("${arg_OUTPUT_FILE}" STREQUAL "")
        message(FATAL_ERROR "OUTPUT_FILE is not set in configure_file_on_build")
    endif()

    add_custom_command(
        OUTPUT ${arg_OUTPUT_FILE}
        COMMAND ${CMAKE_COMMAND}
            -DINPUT_FILE=${arg_INPUT_FILE}
            -DOUTPUT_FILE=${arg_OUTPUT_FILE}
            -DINCLUDE_FILES=${arg_INCLUDES}
            -P ${CMAKE_SOURCE_DIR}/cmake/configure_file_on_build.cmake
        DEPENDS ${arg_INPUT_FILE} ${arg_INCLUDES}
        COMMENT "🔨 Generating ${arg_OUTPUT_FILE}")

    add_custom_target(${arg_TARGET} DEPENDS ${arg_OUTPUT_FILE})
endfunction()
