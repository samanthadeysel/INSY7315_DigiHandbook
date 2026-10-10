package com.example.employeedigitalhandbook.viewmodel

import androidx.lifecycle.LiveData
import androidx.lifecycle.MutableLiveData
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import androidx.lifecycle.viewModelScope
import com.example.employeedigitalhandbook.repositories.AuthRepository
import com.example.employeedigitalhandbook.admin.AuthResult
import com.example.employeedigitalhandbook.sessions.SessionManager
import kotlinx.coroutines.launch

class LoginViewModel(private val sessionManager: SessionManager) : ViewModel() {

    private val repository = AuthRepository(sessionManager)

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

class LoginViewModelFactory(private val sessionManager: SessionManager) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(LoginViewModel::class.java)) {
            @Suppress("UNCHECKED_CAST")
            return LoginViewModel(sessionManager) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}