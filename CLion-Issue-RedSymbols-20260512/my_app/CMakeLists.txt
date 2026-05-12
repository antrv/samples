find_package(Boost CONFIG REQUIRED COMPONENTS asio beast)

set(PROJECT_NAME my_app)

get_source_files(${CMAKE_CURRENT_SOURCE_DIR}/src SOURCE_FILES)
add_executable(${PROJECT_NAME} ${SOURCE_FILES})
add_dependencies(programs ${PROJECT_NAME})

target_include_directories(${PROJECT_NAME} PRIVATE ${CMAKE_CURRENT_SOURCE_DIR}/src)

target_link_libraries(${PROJECT_NAME} PRIVATE
    Boost::boost
)
