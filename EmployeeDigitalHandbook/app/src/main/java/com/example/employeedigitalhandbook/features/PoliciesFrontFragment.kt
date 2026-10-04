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
import com.example.employeedigitalhandbook.adapters.PolicyAdapter
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.data.Policy
import com.example.employeedigitalhandbook.databinding.FragmentPoliciesFrontBinding
import kotlinx.coroutines.launch

class PoliciesFrontFragment : Fragment() {

    private var _binding: FragmentPoliciesFrontBinding? = null
    private val binding get() = _binding!!

    private lateinit var policyAdapter: PolicyAdapter
    private var originalPoliciesList: List<Policy> = emptyList()

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentPoliciesFrontBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        setupRecyclerView()
        setupListeners()
        fetchPolicies()
    }

    private fun setupRecyclerView() {
        policyAdapter = PolicyAdapter(emptyList()) { selectedPolicy ->
            val bundle = bundleOf("POLICY_ID" to selectedPolicy.id)
            findNavController().navigate(
                R.id.action_policiesFrontFragment_to_policiesBackFragment,
                bundle
            )
        }

        binding.recyclerViewPolicies.apply {
            layoutManager = LinearLayoutManager(requireContext())
            adapter = policyAdapter
        }
    }

    private fun setupListeners() {
        binding.backArrowImageView.setOnClickListener {
            findNavController().navigateUp()
        }

        binding.searchEditText.addTextChangedListener(object : TextWatcher {
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}
            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {
                filterPolicies(s?.toString().orEmpty())
            }
            override fun afterTextChanged(s: Editable?) {}
        })
    }

    private fun fetchPolicies() {
        lifecycleScope.launch {
            try {
                val response = ApiClient.apiService.getPolicies()
                val body = response.body()

                if (response.isSuccessful && body != null && body.data != null) {
                    originalPoliciesList = body.data
                    policyAdapter.updateData(originalPoliciesList)
                } else {
                    Toast.makeText(requireContext(), "Failed to load policies", Toast.LENGTH_SHORT).show()
                }
            } catch (e: Exception) {
                Toast.makeText(requireContext(), "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    private fun filterPolicies(query: String) {
        val filtered = if (query.isEmpty()) {
            originalPoliciesList
        } else {
            originalPoliciesList.filter {
                it.title.contains(query, ignoreCase = true) ||
                        it.category.contains(query, ignoreCase = true) ||
                        (it.summary?.contains(query, ignoreCase = true) == true)
            }
        }
        policyAdapter.updateData(filtered)
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}