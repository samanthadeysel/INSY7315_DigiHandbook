package com.example.employeedigitalhandbook.repositories

import com.example.employeedigitalhandbook.data.CommunityEvent

sealed class EventResult {
    data class Success(val events: List<CommunityEvent>) : EventResult()
    data class Error(val message: String) : EventResult()
}