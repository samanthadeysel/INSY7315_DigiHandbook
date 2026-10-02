package com.example.employeedigitalhandbook.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.employeedigitalhandbook.api.ApiClient
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
                val response = ApiClient.apiService.getCommunityEvents()
                if (response.isSuccessful && response.body() != null) {
                    originalEventList = response.body()!!
                    _eventsState.value = EventResult.Success(originalEventList)
                } else {
                    _eventsState.value = EventResult.Error("Failed to fetch events: ${response.code()}")
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