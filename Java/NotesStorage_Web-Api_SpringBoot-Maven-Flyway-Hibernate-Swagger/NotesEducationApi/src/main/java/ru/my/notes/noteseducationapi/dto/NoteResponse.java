package ru.my.notes.noteseducationapi.dto;

import ru.my.notes.noteseducationapi.note.Note;

import java.time.LocalDateTime;

public record NoteResponse(
        Long id,
        String title,
        String text,
        boolean done,
        int priority,
        LocalDateTime createdAt
) {
    public static NoteResponse from(Note note){
        return new NoteResponse(
                note.getId(),
                note.getTitle(),
                note.getText(),
                note.isDone(),
                note.getPriority(),
                note.getCreatedAt()
        );
    }
}
