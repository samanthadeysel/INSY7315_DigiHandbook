package com.example.employeedigitalhandbook.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.example.employeedigitalhandbook.repositories.AuthRepository
import com.example.employeedigitalhandbook.admin.AuthResult
import kotlinx.coroutines.launch

class LoginViewModel : ViewModel() {

    private val repository = AuthRepository()

    private val _loginState = MutableLiveData<AuthResult?>()
    val loginState: LiveData<AuthResult?> = _loginState

    fun login(email: String, password: String) {
        if (email.isBlank() || password.isBlank()) {
            _loginState.value = AuthResult.Error("Please enter both email and password.")
            return
        }

        viewModelScope.launch {
            val result = repository.login(email.trim(), password)
            _loginState.value = result
        }
    }

    fun clearState() {
        _loginState.value = null
    }
}