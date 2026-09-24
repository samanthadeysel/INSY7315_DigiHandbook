package com.example.employeedigitalhandbook.features

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import androidx.navigation.fragment.findNavController
import com.example.employeedigitalhandbook.R

class ComingSoonFragment : Fragment() {

    //var
    private lateinit var btnBackToHome : Button


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
        return inflater.inflate(R.layout.fragment_coming_soon, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        btnBackToHome = view.findViewById(R.id.btnBackToHome)

        btnBackToHome.setOnClickListener {
            //navigate to home page
            findNavController().navigate(R.id.action_comingSoonFragment_to_homePageFragment)
        }

    }
}