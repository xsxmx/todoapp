<template>
  <div v-if="isOpen" class="modal-overlay">
    <div class="modal">
      <form @submit.prevent="submitForm">
        <div class="input-group">
          <input v-model="localTitle" type="text" placeholder="Title" class="note-input title-input" required />
          <textarea v-model="localContent" placeholder="Content" class="note-input content-input" required></textarea>
        </div>
        <div class="edit-buttons">
          <button type="submit" class="note-modal-button">{{ selectedNote ? 'Save' : 'Add' }}</button>
          <button @click="closeModal" type="button" class="note-modal-button">Cancel</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
export default {
  props: ['isOpen', 'selectedNote'],
  data() {
    return {
      localTitle: this.selectedNote ? this.selectedNote.title : '',
      localContent: this.selectedNote ? this.selectedNote.content : '',
    };
  },
  methods: {
    submitForm() {
      this.$emit('submit-form', {
        id: this.selectedNote ? this.selectedNote.id : null,
        title: this.localTitle,
        content: this.localContent,
      });
    },
    closeModal() {
      this.$emit('close-modal');
    },
  },
  watch: {
    selectedNote(newVal) {
      if (newVal) {
        this.localTitle = newVal.title;
        this.localContent = newVal.content;
      } else {
        this.localTitle = '';
        this.localContent = '';
      }
    },
  },
};
</script>

<style lang="scss" scoped>
</style>
