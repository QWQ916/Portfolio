package ru.my.notes.noteseducationapi.note;


import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import ru.my.notes.noteseducationapi.dto.CreateNoteRequest;
import ru.my.notes.noteseducationapi.dto.NoteResponse;
import ru.my.notes.noteseducationapi.dto.NoteStats;
import ru.my.notes.noteseducationapi.dto.UpdateNoteRequest;

import java.util.List;

import io.swagger.v3.oas.annotations.tags.Tag;

@Tag(name = "Заметки", description = "Создание, чтение, изменение и удаление заметок")
@RestController
@RequestMapping("/api/v1/notes")
public class NoteController {

    private final NoteService ns;
    public NoteController(NoteService ns){
        this.ns = ns;
    }

    @GetMapping("/{id}")
    public ResponseEntity<NoteResponse> getSingleNote(@PathVariable long id){
        return ResponseEntity.of(ns.getNoteById(id).map(NoteResponse::from));
    }

    @GetMapping
    public List<NoteResponse> notesSearch(@RequestParam(required = false) String str, @RequestParam(required = false) Boolean done, @RequestParam(required = false, defaultValue = "date") String sort, @RequestParam(defaultValue = "15") long limit){
        return ns.getAndFilterNotes(str, done, sort, limit).stream().map(NoteResponse::from).toList();
    }

    @GetMapping("/count")
    public long count(){
        return ns.notesCount();
    }

    @GetMapping("/stats")
    public NoteStats statistic(){
        return ns.statisticNote();
    }


    @PostMapping
    @ResponseStatus(HttpStatus.CREATED)
    public NoteResponse create(@RequestBody CreateNoteRequest query){
        return NoteResponse.from(ns.createNote(query.title(), query.text()));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<Void> delete(@PathVariable long id){
        return ns.deleteNote(id)
                ? ResponseEntity.noContent().build()   // 204
                : ResponseEntity.notFound().build();   // 404
    }

    @PutMapping("/{id}")
    public ResponseEntity<NoteResponse> update(@PathVariable long id,
                                               @RequestBody UpdateNoteRequest request) {
        return ResponseEntity.of(
                ns.updateNote(id, request.title(), request.text(), request.done())
                        .map(NoteResponse::from));
    }
}
