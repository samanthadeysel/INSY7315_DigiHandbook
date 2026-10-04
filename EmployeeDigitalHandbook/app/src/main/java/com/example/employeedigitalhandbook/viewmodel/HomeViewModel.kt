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
                val body = response.body()
                if (response.isSuccessful && body != null && body.data != null) {
                    _doctorsState.value = DoctorResult.ListSuccess(body.data)
                } else {
                    _doctorsState.value = DoctorResult.Error("Failed to fetch doctors: ${response.code()}")
                }
            } catch (e: Exception) {
                _doctorsState.value = DoctorResult.Error("Network error: ${e.localizedMessage}")
            }
        }
    }
}