package com.example.employeedigitalhandbook.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.employeedigitalhandbook.api.ApiClient.apiService
import com.example.employeedigitalhandbook.data.CommunityEvent
import com.example.employeedigitalhandbook.repositories.EventResult
import kotlinx.coroutines.launch

class CommunityViewModel : ViewModel() {

    private val _eventsState = MutableLiveData<EventResult>()
    val eventsState: LiveData<EventResult> = _eventsState

    private var originalEventList: List<CommunityEvent> = emptyList()

    fun loadEvents() {
        viewModelScope.launch {
            try {
                val response = apiService.getCommunityEvents()
                if (response.isSuccessful && response.body() != null) {
                    val apiResponse = response.body()!!

                    if (apiResponse.success && apiResponse.data != null) {
                        originalEventList = apiResponse.data
                        _eventsState.value = EventResult.Success(originalEventList)
                    } else {
                        _eventsState.value = EventResult.Error(apiResponse.message ?: "Failed to load events")
                    }
                } else {
                    _eventsState.value = EventResult.Error("Server error: ${response.code()}")
                }
            } catch (e: Exception) {
                _eventsState.value = EventResult.Error("Network error: ${e.localizedMessage}")
            }
        }
    }

    fun filterEventsByCategory(category: String) {
        if (category.equals("All", ignoreCase = true)) {
            _eventsState.value = EventResult.Success(originalEventList)
        } else {
            val filtered = originalEventList.filter {
                it.category.equals(category, ignoreCase = true)
            }
            _eventsState.value = EventResult.Success(filtered)
        }
    }
}