package com.example.employeedigitalhandbook

import android.os.Bundle
import android.view.View
import android.view.WindowManager
import android.widget.TextView
import androidx.activity.OnBackPressedCallback
import androidx.activity.enableEdgeToEdge
import androidx.appcompat.app.AppCompatActivity
import androidx.core.view.GravityCompat
import androidx.core.view.ViewCompat
import androidx.core.view.WindowInsetsCompat
import androidx.drawerlayout.widget.DrawerLayout
import androidx.fragment.app.Fragment
import androidx.fragment.app.FragmentManager
import androidx.navigation.NavController
import androidx.navigation.fragment.NavHostFragment
import androidx.navigation.ui.setupWithNavController
import com.example.employeedigitalhandbook.session.SessionManager
import kotlin.jvm.java

class MainActivity : AppCompatActivity() {

    private lateinit var drawerLayout: DrawerLayout
    private lateinit var navController: NavController

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)

        drawerLayout = findViewById(R.id.main)

        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.nav_host_fragment)) { v, insets ->
            val systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars())
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, 0)
            insets
        }

        val backPressedCallback = object : OnBackPressedCallback(false) {
            override fun handleOnBackPressed() {
                if (drawerLayout.isDrawerOpen(GravityCompat.END)) {
                    drawerLayout.closeDrawer(GravityCompat.END)
                }
            }
        }

        onBackPressedDispatcher.addCallback(this, backPressedCallback)

        drawerLayout.addDrawerListener(object : DrawerLayout.SimpleDrawerListener() {
            override fun onDrawerOpened(drawerView: View) {
                backPressedCallback.isEnabled = true
            }

            override fun onDrawerClosed(drawerView: View) {
                backPressedCallback.isEnabled = false
            }
        })

        //val bottomNav = findViewById<BottomNavigationView>(R.id.bottomNavigation)

        val navHostFragment = supportFragmentManager
            .findFragmentById(R.id.nav_host_fragment) as NavHostFragment

             navController = navHostFragment.navController

        //bottomNav.setupWithNavController(navController)

        navController.addOnDestinationChangedListener { _, destination, _ ->
            when (destination.id) {

                R.id.loginFragment -> {
                    drawerLayout.setDrawerLockMode(DrawerLayout.LOCK_MODE_LOCKED_CLOSED)
//                R.id.homePageFragment,
//                R.id.nav_home,
//                R.id.nav_messages,
//                R.id.nav_menu,
//                R.id.policiesFrontFragment -> {
                    //bottomNav.visibility = View.VISIBLE
                }
                else -> {
                    //bottomNav.visibility = View.GONE
                    drawerLayout.setDrawerLockMode(DrawerLayout.LOCK_MODE_UNLOCKED)
                }
            }
        }

        setupDrawerMenu()

        window.setFlags(
            WindowManager.LayoutParams.FLAG_SECURE,
            WindowManager.LayoutParams.FLAG_SECURE
        )
    }

    private fun setupDrawerMenu () {findViewById<TextView>(R.id.menuQuiz).setOnClickListener { //[cite: 13]
        drawerLayout.closeDrawer(GravityCompat.END)
        navController.navigate(R.id.quizListFragment)
    }

        findViewById<TextView>(R.id.menuCpdInfo).setOnClickListener {
            drawerLayout.closeDrawer(GravityCompat.END)
            navController.navigate(R.id.resourcesFrontFragment)
            //ignore the name - goes to resources
        }

        findViewById<TextView>(R.id.menuPolicies).setOnClickListener {
            drawerLayout.closeDrawer(GravityCompat.END)
            navController.navigate(R.id.policiesFrontFragment)
        }

        findViewById<TextView>(R.id.menuLogOut).setOnClickListener {
            drawerLayout.closeDrawer(GravityCompat.END)
            // Clear session/tokens and return to login:
            navController.navigate(R.id.loginFragment)
        }
    }

    // Helper method so fragments can open the drawer via a hamburger icon
    fun openMenu() {
        if (!drawerLayout.isDrawerOpen(GravityCompat.END)) {
            drawerLayout.openDrawer(GravityCompat.END)
        }
    }

}