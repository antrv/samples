#pragma once

#include <boost/asio/awaitable.hpp>
#include <boost/asio/co_spawn.hpp>
#include <boost/asio/experimental/awaitable_operators.hpp>
#include <boost/asio/experimental/channel.hpp>

#include <boost/beast/http/verb.hpp>

#include <print>

template <class T, class Executor = boost::asio::any_io_executor>
using Awaitable = boost::asio::awaitable<T, Executor>; // Clangd: No member named 'awaitable' in namespace 'boost::asio'

using boost::asio::co_spawn; // Clangd: No member named 'co_spawn' in namespace 'boost::asio'
using boost::asio::experimental::awaitable_operators::operator&&;
using boost::asio::experimental::awaitable_operators::operator||;

namespace this_coro = boost::asio::this_coro;

namespace coro {
using boost::asio::use_awaitable; // Clangd: No member named 'use_awaitable' in namespace 'boost::asio'
} // namespace coro

Awaitable<int> asyncAdd(const int a, const int b) // Clangd: No template named 'Awaitable'
{
    co_return a + b; // Clangd: Std::coroutine_traits type was not found; include <coroutine> before defining a coroutine
}

// Symbols with red underscore above: `awaitable`, `co_spawn`, `use_awaitable`, `Awaitable`, `co_return`;

using HttpMethod = boost::beast::http::verb;

int main()
{
    // Red `post`:
    std::println("{}", boost::beast::http::to_string(boost::beast::http::verb::post)); // Cannot resolve symbol 'post'
    std::println("{}", boost::beast::http::to_string(HttpMethod::post)); // Cannot resolve symbol 'post'

    return 0;
}
