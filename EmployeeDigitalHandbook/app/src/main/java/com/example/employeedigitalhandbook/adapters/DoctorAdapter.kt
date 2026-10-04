package com.example.employeedigitalhandbook.adapters

import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.recyclerview.widget.RecyclerView
import com.bumptech.glide.Glide
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.data.Doctor

class DoctorAdapter(
    private val doctors: List<Doctor>,
    private val onDoctorClick: (Doctor) -> Unit
) : RecyclerView.Adapter<DoctorAdapter.DoctorViewHolder>() {

    class DoctorViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        val doctorImage: ImageView = itemView.findViewById(R.id.doctorImageView)
        val doctorName: TextView = itemView.findViewById(R.id.doctorNameTextView)
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): DoctorViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_doctor_card, parent, false)
        return DoctorViewHolder(view)
    }

    override fun onBindViewHolder(holder: DoctorViewHolder, position: Int) {
        val doctor = doctors[position]

        holder.doctorName.text = doctor.fullNameWithTitle

        if (!doctor.doctorImg.isNullOrBlank()) {
            Glide.with(holder.itemView.context)
                .load(doctor.doctorImg)
                .into(holder.doctorImage)
        } else {
            holder.doctorImage.setImageResource(R.drawable.placeholder_doctor)
        }

        holder.itemView.setOnClickListener { onDoctorClick(doctor) }
    }

    override fun getItemCount(): Int = doctors.size
}