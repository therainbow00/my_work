package com.example.spring;

import org.springframework.data.jpa.repository.JpaRepository;

public interface SpringRepository extends JpaRepository<springModel, Integer>
{
    
}
