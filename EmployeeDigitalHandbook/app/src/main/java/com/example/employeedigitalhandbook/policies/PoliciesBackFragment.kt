package com.example.employeedigitalhandbook.policies

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import androidx.navigation.fragment.findNavController
import com.example.employeedigitalhandbook.R
import com.google.android.material.bottomnavigation.BottomNavigationView

class PoliciesBackFragment : Fragment() {

    //var
    private lateinit var backArrowImageView: ImageView

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        arguments?.let {
        }
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // Inflate the layout for this fragment
        return inflater.inflate(R.layout.fragment_policies_back, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val bottomNav = requireActivity().findViewById<BottomNavigationView>(R.id.bottomNavigation)
        bottomNav.visibility = View.VISIBLE

        //back to policies front
        val backArrowImageView = view.findViewById<ImageView>(R.id.backArrowImageView)

        backArrowImageView.setOnClickListener {
            findNavController().navigate(R.id.action_policiesBackFragment_to_policiesFrontFragment)
        }
    }
}