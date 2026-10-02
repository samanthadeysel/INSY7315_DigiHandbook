package com.example.employeedigitalhandbook.features

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.data.BragBook
import com.example.employeedigitalhandbook.databinding.FragmentCreateBragBinding
import kotlinx.coroutines.launch

class CreateBragFragment : Fragment() {

    private var _binding: FragmentCreateBragBinding? = null
    private val binding get() = _binding!!

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentCreateBragBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        binding.btnBackBrag.setOnClickListener {
            findNavController().navigateUp()
        }

        binding.btnSubmitBrag.setOnClickListener {
            submitBragPost()
        }
    }

    private fun submitBragPost() {
        val recipient = binding.edtRecipient.text?.toString()?.trim().orEmpty()
        val message = binding.edtMessage.text?.toString()?.trim().orEmpty()
        val isAnonymous = binding.switchAnonymous.isChecked

        if (recipient.isEmpty()) {
            binding.inputLayoutRecipient.error = "Please enter who you are complimenting"
            return
        } else {
            binding.inputLayoutRecipient.error = null
        }

        if (message.isEmpty()) {
            binding.inputLayoutMessage.error = "Please write a compliment"
            return
        } else {
            binding.inputLayoutMessage.error = null
        }

        val newPost = BragBook(
            recipientName = recipient,
            content = message,
            senderType = if (isAnonymous) "Anonymous" else "Peer",
            isAnonymous = isAnonymous
        )

        binding.btnSubmitBrag.isEnabled = false

        lifecycleScope.launch {
            try {
                val response = ApiClient.apiService.createBragPost(newPost)
                if (response.isSuccessful) {
                    Toast.makeText(requireContext(), "Compliment posted successfully!", Toast.LENGTH_SHORT).show()
                    findNavController().navigateUp()
                } else {
                    binding.btnSubmitBrag.isEnabled = true
                    Toast.makeText(requireContext(), "Failed to post compliment", Toast.LENGTH_SHORT).show()
                }
            } catch (e: Exception) {
                binding.btnSubmitBrag.isEnabled = true
                Toast.makeText(requireContext(), "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}