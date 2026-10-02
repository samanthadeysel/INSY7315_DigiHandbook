package com.example.employeedigitalhandbook.features

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.webkit.WebChromeClient
import android.webkit.WebView
import android.webkit.WebViewClient
import android.widget.Toast
import androidx.fragment.app.Fragment
import androidx.lifecycle.lifecycleScope
import androidx.navigation.fragment.findNavController
import com.example.employeedigitalhandbook.api.ApiClient
import com.example.employeedigitalhandbook.databinding.FragmentPoliciesBackBinding
import kotlinx.coroutines.launch
import java.net.URLEncoder

class PoliciesBackFragment : Fragment() {

    private var _binding: FragmentPoliciesBackBinding? = null
    private val binding get() = _binding!!

    private var policyId: Int = -1

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        policyId = arguments?.getInt("POLICY_ID", -1) ?: -1
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        _binding = FragmentPoliciesBackBinding.inflate(inflater, container, false)
        return binding.root
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        setupWebView()
        setupControls()

        if (policyId != -1) {
            fetchAndLoadPolicyDetails(policyId)
        } else {
            Toast.makeText(requireContext(), "Invalid Policy ID", Toast.LENGTH_SHORT).show()
        }
    }

    private fun setupWebView() {
        binding.pdfWebView.apply {
            settings.javaScriptEnabled = true
            settings.domStorageEnabled = true
            settings.builtInZoomControls = true
            settings.displayZoomControls = false

            webViewClient = object : WebViewClient() {
                override fun onPageFinished(view: WebView?, url: String?) {
                    super.onPageFinished(view, url)
                    _binding?.pdfProgressBar?.visibility = View.GONE
                }
            }

            webChromeClient = object : WebChromeClient() {
                override fun onProgressChanged(view: WebView?, newProgress: Int) {
                    if (newProgress < 100) {
                        _binding?.pdfProgressBar?.visibility = View.VISIBLE
                    } else {
                        _binding?.pdfProgressBar?.visibility = View.GONE
                    }
                }
            }
        }
    }

    private fun setupControls() {
        binding.backArrowImageView.setOnClickListener {
            findNavController().navigateUp()
        }

        binding.previousPageIcon.setOnClickListener {
            if (binding.pdfWebView.canGoBack()) {
                binding.pdfWebView.goBack()
            }
        }

        binding.nextPageIcon.setOnClickListener {
            if (binding.pdfWebView.canGoForward()) {
                binding.pdfWebView.goForward()
            }
        }

        binding.searchPdfIcon.setOnClickListener {
            binding.pdfWebView.showFindDialog(null, true)
        }
    }

    private fun fetchAndLoadPolicyDetails(id: Int) {
        binding.pdfProgressBar.visibility = View.VISIBLE

        lifecycleScope.launch {
            try {
                val response = ApiClient.apiService.getPolicyById(id)
                if (response.isSuccessful && response.body() != null) {
                    val policy = response.body()!!

                    binding.pageTitleTextView.text = policy.category
                    binding.breadcrumbTextView.text = policy.breadcrumbPath
                        ?: "../${policy.category}/${policy.title}"

                    val rawPdfUrl = policy.pdfUrl
                    if (!rawPdfUrl.isNullOrEmpty()) {
                        // Render PDF via Google Drive/Docs viewer inside WebView
                        val encodedUrl = URLEncoder.encode(rawPdfUrl, "UTF-8")
                        val webViewUrl = "https://docs.google.com/gview?embedded=true&url=$encodedUrl"
                        binding.pdfWebView.loadUrl(webViewUrl)
                    } else {
                        binding.pdfProgressBar.visibility = View.GONE
                        Toast.makeText(requireContext(), "No PDF document attached", Toast.LENGTH_SHORT).show()
                    }
                } else {
                    binding.pdfProgressBar.visibility = View.GONE
                    Toast.makeText(requireContext(), "Failed to fetch policy details", Toast.LENGTH_SHORT).show()
                }
            } catch (e: Exception) {
                binding.pdfProgressBar.visibility = View.GONE
                Toast.makeText(requireContext(), "Error: ${e.localizedMessage}", Toast.LENGTH_SHORT).show()
            }
        }
    }

    override fun onDestroyView() {
        super.onDestroyView()
        _binding = null
    }
}