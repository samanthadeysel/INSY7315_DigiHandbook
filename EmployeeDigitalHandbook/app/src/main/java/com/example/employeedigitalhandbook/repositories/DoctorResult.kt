package com.example.employeedigitalhandbook.repositories

import com.example.employeedigitalhandbook.data.Doctor

sealed class DoctorResult {
    data class Success(val doctor: Doctor) : DoctorResult()
    data class ListSuccess(val doctors: List<Doctor>) : DoctorResult()
    data class Error(val message: String) : DoctorResult()
}