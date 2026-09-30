package com.example.employeedigitalhandbook.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.api.ApiDoctor

class DoctorAdapter(
    private val doctorList: List<ApiDoctor>,
    private val onDoctorClick: (ApiDoctor) -> Unit
) : RecyclerView.Adapter<DoctorAdapter.DoctorViewHolder>() {

    class DoctorViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val doctorImageView: ImageView = itemView.findViewById(R.id.doctorImageView)
        val doctorNameTextView: TextView = itemView.findViewById(R.id.doctorNameTextView)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): DoctorViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_doctor_card, parent, false)
        return DoctorViewHolder(view)
    }

    override fun onBindViewHolder(holder: DoctorViewHolder, position: Int) {
        val doctor = doctorList[position]

        holder.doctorNameTextView.text = doctor.fullNameWithTitle

        holder.doctorImageView.setImageResource(R.drawable.peh_logo_black_wording_portrait)

        holder.itemView.setOnClickListener {
            onDoctorClick(doctor)
        }
    }

    override fun getItemCount(): Int = doctorList.size
}