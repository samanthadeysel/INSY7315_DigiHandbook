package com.example.employeedigitalhandbook.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.data.BragBook
import com.example.employeedigitalhandbook.repositories.BragBookResult
import kotlinx.coroutines.launch

class BragBookViewModel : ViewModel() {

    private val _bragBookState = MutableLiveData<BragBookResult>()
    val bragBookState: LiveData<BragBookResult> = _bragBookState

    fun loadBragPosts() {
        viewModelScope.launch {
            try {
                val response = ApiClient.apiService.getBragPosts()
                val body = response.body()

                if (response.isSuccessful && body != null && body.data != null) {
                    _bragBookState.value = BragBookResult.Success(body.data)
                } else {
                    _bragBookState.value = BragBookResult.Error("Failed to retrieve notes: ${response.code()}")
                }
            } catch (e: Exception) {
                _bragBookState.value = BragBookResult.Error("Network error: ${e.localizedMessage}")
            }
        }
    }

    fun createBragPost(post: BragBook) {
        viewModelScope.launch {
            try {
                val response = ApiClient.apiService.createBragPost(post)
                if (response.isSuccessful) {
                    loadBragPosts() // Refresh list on success
                } else {
                    _bragBookState.value = BragBookResult.Error("Failed to save note")
                }
            } catch (e: Exception) {
                _bragBookState.value = BragBookResult.Error("Network error: ${e.localizedMessage}")
            }
        }
    }
}