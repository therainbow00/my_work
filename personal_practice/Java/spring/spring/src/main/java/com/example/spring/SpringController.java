package com.example.spring;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import java.util.Random;

@Controller
public class SpringController
{
    @GetMapping("index")
    public String index(Model model)
    {
        Random random = new Random();
        springModel test = new springModel();
        test.setName("index");
        test.setAge(random.nextInt(100));

        model.addAttribute("test", test);
        return "index";
    }
}
