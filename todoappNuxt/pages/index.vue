<template>
  <div :class="showApp ? 'App app-fade-in' : 'App'">
    <div v-if="!showApp" class="start-container">
      <img src="/logo.png" alt="Logo" />
      <button @click="setShowApp(true)" class="start-button">Get started</button>
    </div>
    <div v-else class="app-container">
      <div class="header-section">
        <div class="search-bar-container">
          <input v-model="searchTerm" type="text" placeholder="Search" class="search-bar" />
        </div>
        <img src="/logo.png" alt="Logo" class="app-logo" />
        <button @click="openModal()" class="add-note-button">
          <i class="bi bi-plus-circle-fill"></i>
        </button>
      </div>
      <div class="main-section">
        <div class="notes-grid">
          <div v-for="note in filteredNotes" :key="note.id" class="note-item">
            <div class="notes-header">
              <button @click="deleteNote(note.id)" class="delete-button">
                <i class="bi bi-x-circle-fill"></i>
              </button>
            </div>
            <h2>{{ note.title }}</h2>
            <p>{{ note.content }}</p>
            <div class="main-buttons-container">
              <button @click="openModal(note)" class="edit-button">
                <i class="bi bi-pencil-fill"></i>
              </button>
            </div>
          </div>
        </div>
      </div>
      <div class="pagination">
        <button v-if="currentPage > 1" @click="setCurrentPage(currentPage - 1)" class="pagination-button-left">
          <i class="bi bi-arrow-left-short"></i>
        </button>
        <button v-if="currentPage * pageSize < notes.length" @click="setCurrentPage(currentPage + 1)" class="pagination-button-right">
          <i class="bi bi-arrow-right-short"></i>
        </button>
      </div>
    </div>
    <NoteModal :is-open="isModalOpen" :selected-note="selectedNote" @close-modal="closeModal" @submit-form="handleAddOrUpdateNote"/>
  </div>
</template>

<script>
import axios from 'axios';
import NoteModal from '@/components/NoteModal.vue';

export default {
  components: {
    NoteModal
  },
  data() {
    return {
      showApp: false,
      notes: [],
      searchTerm: '',
      selectedNote: null,
      isModalOpen: false,
      currentPage: 1,
      pageSize: 10,
    };
  },
  computed: {
    filteredNotes() {
      return this.notes.filter(note =>
        note.title.toLowerCase().includes(this.searchTerm.toLowerCase()) ||
        note.content.toLowerCase().includes(this.searchTerm.toLowerCase())
      ).slice((this.currentPage - 1) * this.pageSize, this.currentPage * this.pageSize);
    },
  },
  methods: {
    setShowApp(value) {
      this.showApp = value;
    },
    openModal(note = null) {
      this.selectedNote = note;
      this.isModalOpen = true;
    },
    closeModal() {
      this.isModalOpen = false;
      this.selectedNote = null;
    },
    async fetchNotes() {
      try {
        const response = await axios.get('http://localhost:5087/api/todoapp/GetNotes');
        this.notes = response.data.map(note => ({
          id: note.id,
          title: note.title,
          content: note.description,
        }));
      } catch (error) {
        console.error('Failed to fetch notes:', error);
      }
    },
    async handleAddOrUpdateNote(note) {
      const method = note.id ? 'put' : 'post';
      const url = note.id
        ? `http://localhost:5087/api/todoapp/EditNote/${note.id}`
        : 'http://localhost:5087/api/todoapp/AddNotes';
      
      try {
        await axios[method](url, {
          title: note.title,
          description: note.content,
        });
        await this.fetchNotes();
        this.closeModal();
      } catch (error) {
        console.error('Error handling note:', error);
      }
    },
    async deleteNote(noteId) {
      try {
        await axios.delete(`http://localhost:5087/api/todoapp/DeleteNotes?id=${noteId}`);
        await this.fetchNotes();
      } catch (error) {
        console.error('Failed to delete note:', error);
      }
    },
    setCurrentPage(page) {
      this.currentPage = page;
    },
  },
  mounted() {
    this.fetchNotes();
  },
};
</script>

<style lang="scss" scoped>
</style>
