package com.example.spring;

import jakarta.persistence.*;
import lombok.*;

@Getter
@Setter
@Entity
public class springModel
{
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private int id;

    private String name;
    private int age;
}