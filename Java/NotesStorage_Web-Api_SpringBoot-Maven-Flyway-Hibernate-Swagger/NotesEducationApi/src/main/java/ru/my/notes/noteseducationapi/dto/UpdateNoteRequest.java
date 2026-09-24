package ru.my.notes.noteseducationapi.dto;

public record UpdateNoteRequest(String title, String text, boolean done) {
}
