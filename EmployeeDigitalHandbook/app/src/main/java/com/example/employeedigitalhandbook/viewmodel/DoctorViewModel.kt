package com.example.employeedigitalhandbook.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.repositories.DoctorResult
import kotlinx.coroutines.launch

class DoctorViewModel : ViewModel() {

    private val _doctorDetailsState = MutableLiveData<DoctorResult>()
    val doctorDetailsState: LiveData<DoctorResult> = _doctorDetailsState

    private val _doctorsListState = MutableLiveData<DoctorResult>()
    val doctorsListState: LiveData<DoctorResult> = _doctorsListState

    fun loadDoctorDetails(doctorId: Int) {
        viewModelScope.launch {
            try {
                val response = ApiClient.apiService.getDoctorById(doctorId)
                val body = response.body()
                if (response.isSuccessful && body != null && body.data != null) {
                    _doctorDetailsState.value = DoctorResult.Success(body.data)
                } else {
                    _doctorDetailsState.value = DoctorResult.Error("Failed to fetch doctor details: ${response.code()}")
                }
            } catch (e: Exception) {
                _doctorDetailsState.value = DoctorResult.Error("Network error: ${e.localizedMessage}")
            }
        }
    }

    fun loadAllDoctors() {
        viewModelScope.launch {
            try {
                val response = ApiClient.apiService.getDoctors()
                val body = response.body()
                if (response.isSuccessful && body != null && body.data != null) {
                    _doctorsListState.value = DoctorResult.ListSuccess(body.data)
                } else {
                    _doctorsListState.value = DoctorResult.Error("Failed to fetch doctors: ${response.code()}")
                }
            } catch (e: Exception) {
                _doctorsListState.value = DoctorResult.Error("Network error: ${e.localizedMessage}")
            }
        }
    }
}