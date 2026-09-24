package ru.my.notes.noteseducationapi.repository;

import org.springframework.data.jpa.repository.JpaRepository;
import ru.my.notes.noteseducationapi.note.Note;

import java.time.LocalDate;
import java.time.LocalDateTime;
import java.util.List;

public interface NoteRepository extends JpaRepository<Note, Long> {

    List<Note> findByTitleContainingIgnoreCase(String part);

    List<Note> findByDone(boolean done);

    List<Note> findByCreatedAtAfter(LocalDateTime moment);

    long countByDone(boolean done);
}
