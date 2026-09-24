package ru.my.notes.noteseducationapi;

import io.swagger.v3.oas.annotations.OpenAPIDefinition;
import io.swagger.v3.oas.annotations.info.Info;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

@OpenAPIDefinition(info = @Info(
        title = "Notes API",
        version = "1.0",
        description = "REST API для заметок. Учебный проект на Spring Boot."
))

@SpringBootApplication
public class NotesEducationApiApplication {

    public static void main(String[] args) {
        SpringApplication.run(NotesEducationApiApplication.class, args);
    }

}
