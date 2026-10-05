package com.example.employeedigitalhandbook.adapters

import android.graphics.Color
import android.view.LayoutInflater
import android.view.ViewGroup
import androidx.recyclerview.widget.RecyclerView
import com.example.employeedigitalhandbook.data.BragBook
import com.example.employeedigitalhandbook.databinding.ItemBragCardBinding

class BragBookAdapter(
    private var posts: List<BragBook>
) : RecyclerView.Adapter<BragBookAdapter.BragViewHolder>() {

    private val cardColors = listOf(
        "#2690CF",
        "#8BC34A",
        "#F57C00"
    )

    class BragViewHolder(val binding: ItemBragCardBinding) :
        RecyclerView.ViewHolder(binding.root)

    override fun onCreateViewHolder(parent: ViewGroup, viewType: Int): BragViewHolder {
        val binding = ItemBragCardBinding.inflate(
            LayoutInflater.from(parent.context),
            parent,
            false
        )
        return BragViewHolder(binding)
    }

    override fun onBindViewHolder(holder: BragViewHolder, position: Int) {
        val post = posts[position]

        holder.binding.txtBragRecipient.text = post.recipientName
        holder.binding.txtBragDate.text = post.datePosted?.ifBlank { "Today" } ?: "Today"
        holder.binding.txtBragMessage.text = post.content

        holder.binding.txtBragSender.text = if (post.isAnonymous) {
            "- Anonymous Peer"
        } else {
            "- ${post.senderType}"
        }

        val colorHex = cardColors[position % cardColors.size]
        holder.binding.cardPostIt.setCardBackgroundColor(Color.parseColor(colorHex))
    }

    override fun getItemCount(): Int = posts.size

    fun updateData(newPosts: List<BragBook>) {
        posts = newPosts
        notifyDataSetChanged()
    }
}