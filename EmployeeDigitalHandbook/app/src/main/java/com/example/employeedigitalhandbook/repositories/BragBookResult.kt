package com.example.employeedigitalhandbook.repositories

import com.example.employeedigitalhandbook.data.BragBook

sealed class BragBookResult {
    data class Success(val posts: List<BragBook>) : BragBookResult()
    data class Error(val message: String) : BragBookResult()
}