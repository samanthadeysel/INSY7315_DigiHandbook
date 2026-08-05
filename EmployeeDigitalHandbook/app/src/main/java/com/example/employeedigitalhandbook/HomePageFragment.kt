package com.example.employeedigitalhandbook

import android.os.Bundle
import androidx.fragment.app.Fragment
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.google.android.material.card.MaterialCardView

//var
class HomePageFragment : Fragment() {
    private lateinit var policyCardOne : MaterialCardView
    private lateinit var policyCardTwo : MaterialCardView
    private lateinit var policyCardThree : MaterialCardView
    private lateinit var policyCardFour : MaterialCardView
    private lateinit var doctorsRecyclerView : RecyclerView

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
        return inflater.inflate(R.layout.fragment_home_page, container, false)
    }


}