package ru.my.notes.noteseducationapi.note;

import org.springframework.stereotype.Service;
import ru.my.notes.noteseducationapi.dto.NoteStats;
import ru.my.notes.noteseducationapi.repository.NoteRepository;

import java.util.Comparator;
import java.util.List;
import java.util.Optional;

@Service
public class NoteService {

    private final NoteRepository notesRepo;
    public NoteService(NoteRepository notesRepo){
        this.notesRepo = notesRepo;
    }

    public List<Note> getAndFilterNotes(String s, Boolean done, String sort, long limit){
        var notes = notesRepo.findAll();
        var query = notes.stream();
        if (s != null && !s.isBlank()) { query = query.filter(n -> n.getTitle().toLowerCase().contains(s.toLowerCase())); }
        if (done != null){ query = query.filter(p -> p.isDone() == done); }
        if (sort.equals("date")) { query = query.sorted(Comparator.comparing(Note::getCreatedAt));}
        else if (sort.equals("title")) { query = query.sorted(Comparator.comparing(Note::getTitle));}
        query = query.limit(limit);
        return query.toList();
    }

    public Optional<Note> getNoteById(long id){
        return notesRepo.findById(id);
    }

    public long notesCount(){
        return notesRepo.count();
    }

    public Note createNote(String title, String text){
        Note note = new Note(title, text);
        return notesRepo.save(note);
    }

    public boolean deleteNote(long id){
        if (!notesRepo.existsById(id)){
            return false;
        }
        notesRepo.deleteById(id);
        return true;
    }

    public NoteStats statisticNote(){
        return new NoteStats(notesRepo.count(), notesRepo.countByDone(true));
    }

    public Optional<Note> updateNote(long id, String title, String text, boolean done) {
        return notesRepo.findById(id).map(note -> {
            note.setTitle(title);
            note.setText(text);
            note.setDone(done);
            return notesRepo.save(note);
        });
    }
}
