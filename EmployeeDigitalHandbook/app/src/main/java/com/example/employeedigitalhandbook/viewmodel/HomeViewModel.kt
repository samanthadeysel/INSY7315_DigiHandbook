package com.example.employeedigitalhandbook.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.employeedigitalhandbook.repositories.DoctorRepository
import com.example.employeedigitalhandbook.repositories.DoctorResult
import kotlinx.coroutines.launch

class HomeViewModel : ViewModel() {

    private val repository = DoctorRepository()

    private val _doctorsState = MutableLiveData<DoctorResult>()
    val doctorsState: LiveData<DoctorResult> = _doctorsState

    fun loadDoctors() {
        viewModelScope.launch {
            val result = repository.fetchDoctors()
            _doctorsState.value = result
        }
    }
}