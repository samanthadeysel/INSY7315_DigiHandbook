package com.example.employeedigitalhandbook.features

import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Toast
import androidx.core.os.bundleOf
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import androidx.recyclerview.widget.LinearLayoutManager
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.adapters.ResourceAdapter
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.data.Resource
import com.example.employeedigitalhandbook.databinding.FragmentResourcesFrontBinding
import kotlinx.coroutines.launch

class ResourcesFrontFragment : Fragment() {

    private var _binding: FragmentResourcesFrontBinding? = null
    private val binding get() = _binding!!

    private lateinit var resourceAdapter: ResourceAdapter
    private var originalList: List<Resource> = emptyList()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentResourcesFrontBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        setupRecyclerView()
        setupListeners()
        fetchResources()
    }

    private fun setupRecyclerView() {
        resourceAdapter = ResourceAdapter(emptyList()) { selectedResource ->
            val bundle = bundleOf("RESOURCE_ID" to selectedResource.id)
            findNavController().navigate(
                R.id.action_resourcesFrontFragment_to_resourcesBackFragment,
                bundle
            )
        }

        binding.recyclerViewResources.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = resourceAdapter
        }
    }

    private fun setupListeners() {
        binding.backArrowImageView.setOnClickListener {
            requireActivity().onBackPressedDispatcher.onBackPressed()
        }

        binding.searchEditText.addTextChangedListener(object : TextWatcher {
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}
            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {
                filterList(s?.toString().orEmpty())
            }
            override fun afterTextChanged(s: Editable?) {}
        })
    }

    private fun fetchResources() {
        lifecycleScope.launch {
            try {
                val response = ApiClient.apiService.getResources()
                if (response.isSuccessful && response.body() != null) {
                    val apiResponse = response.body()!!

                    if (apiResponse.success && apiResponse.data != null) {
                        originalList = apiResponse.data
                        resourceAdapter.updateData(originalList)
                    } else {
                        Toast.makeText(
                            requireContext(),
                            apiResponse.message ?: "Failed to load resources",
                            Toast.LENGTH_SHORT
                        ).show()
                    }
                } else {
                    Toast.makeText(requireContext(), "Failed to load resources", Toast.LENGTH_SHORT).show()
                }
            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
            }
        }
    }
    private fun filterList(query: String) {
        val filtered = if (query.isEmpty()) {
            originalList
        } else {
            originalList.filter {
                it.title.contains(query, ignoreCase = true) ||
                        it.category.contains(query, ignoreCase = true) ||
                        (it.description?.contains(query, ignoreCase = true) == true)
            }
        }
        resourceAdapter.updateData(filtered)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}