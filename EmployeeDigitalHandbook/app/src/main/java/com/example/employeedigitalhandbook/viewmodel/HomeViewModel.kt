package com.example.employeedigitalhandbook.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.repositories.DoctorResult
import kotlinx.coroutines.launch

class HomeViewModel : ViewModel() {

    private val _doctorsState = MutableLiveData<DoctorResult>()
    val doctorsState: LiveData<DoctorResult> = _doctorsState

    fun loadDoctors() {
        viewModelScope.launch {
            try {
                val response = ApiClient.apiService.getDoctors()
                if (response.isSuccessful && response.body() != null) {
                    _doctorsState.value = DoctorResult.ListSuccess(response.body()!!)
                } else {
                    _doctorsState.value = DoctorResult.Error("Failed to fetch doctors: ${response.code()}")
                }
            } catch (e: Exception) {
                _doctorsState.value = DoctorResult.Error("Network error: ${e.localizedMessage}")
            }
        }
    }
}