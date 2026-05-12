cmake_minimum_required(VERSION 4.0 FATAL_ERROR)

foreach(INCLUDE_FILE ${INCLUDE_FILES})
    include(${INCLUDE_FILE})
endforeach()

# Configure template
configure_file(${INPUT_FILE} ${OUTPUT_FILE} NEWLINE_STYLE LF)
