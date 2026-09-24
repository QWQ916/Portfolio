package ru.my.notes.noteseducationapi.note;

import jakarta.persistence.*;

import java.time.LocalDateTime;

@Entity
@Table(name = "notes")
public class Note{

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    @Column(nullable = false)
    private String title;

    private String text;

    @Column(nullable = false)
    private boolean done;

    private int priority;

    @Column(nullable = false)
    private LocalDateTime createdAt;

    public Note(String title, String text){
        this.title = title; this.text = text; this.done = false; this.createdAt = LocalDateTime.now();
    }

    protected Note(){}

    public Long getId() {return id;}
    public String getTitle() {return title;}
    public String getText() {return text;}
    public LocalDateTime getCreatedAt() {return createdAt;}
    public int getPriority() {return priority;}
    public boolean isDone() {return done;}

    public void setTitle(String title) { this.title = title; }
    public void setText(String text) { this.text = text; }
    public void setDone(boolean done) { this.done = done; }
    public void setPriority(int priority){this.priority = priority;}

}
