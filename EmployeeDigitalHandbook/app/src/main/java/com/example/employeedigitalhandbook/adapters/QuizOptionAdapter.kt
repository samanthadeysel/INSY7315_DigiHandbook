package com.example.employeedigitalhandbook.adapters

import android.graphics.Color
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.ImageView
import android.widget.TextView
import androidx.core.content.ContextCompat
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.R
import com.example.employeedigitalhandbook.data.QuizOption
import com.google.android.material.card.MaterialCardView

class QuizOptionAdapter(
    private var optionList: List<QuizOption>,
    private val onOptionClick: (QuizOption) -> Unit
) : RecyclerView.Adapter<QuizOptionAdapter.OptionViewHolder>() {

    private var selectedPosition = RecyclerView.NO_POSITION

    inner class OptionViewHolder(itemView: View) : RecyclerView.ViewHolder(itemView) {
        private val cardOption: MaterialCardView = itemView.findViewById(R.id.cardOption)
        private val imgRadioCheck: ImageView = itemView.findViewById(R.id.imgRadioCheck)
        private val txtOptionText: TextView = itemView.findViewById(R.id.txtOptionText)

        fun bind(option: QuizOption, position: Int) {
            txtOptionText.text = option.optionText

            val isSelected = position == selectedPosition

            if (isSelected) {
                cardOption.strokeColor = ContextCompat.getColor(itemView.context, R.color.teal_700)
                imgRadioCheck.setColorFilter(ContextCompat.getColor(itemView.context, R.color.teal_700))
            } else {
                cardOption.strokeColor = Color.parseColor("#CBD5E1")
                imgRadioCheck.setColorFilter(Color.parseColor("#94A3B8"))
            }

            itemView.setOnClickListener {
                val previousSelected = selectedPosition
                selectedPosition = bindingAdapterPosition

                notifyItemChanged(previousSelected)
                notifyItemChanged(selectedPosition)

                onOptionClick(option)
            }
        }
    }

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): OptionViewHolder {
        val view = LayoutInflater.from(parent.context)
            .inflate(R.layout.item_quiz_option, parent, false)
        return OptionViewHolder(view)
    }

    override fun onBindViewHolder(holder: OptionViewHolder, position: Int) {
        holder.bind(optionList[position], position)
    }

    override fun getItemCount(): Int = optionList.size

    fun updateOptions(newList: List<QuizOption>) {
        this.optionList = newList
        this.selectedPosition = RecyclerView.NO_POSITION
        notifyDataSetChanged()
    }

    fun getSelectedOption(): QuizOption? {
        return if (selectedPosition in optionList.indices) optionList[selectedPosition] else null
    }
}