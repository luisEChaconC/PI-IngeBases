<template>
  <div class="export-dropdown position-relative">
    <button 
      class="btn btn-primary dropdown-toggle" 
      type="button" 
      @click="toggleDropdown"
    >
      Exportar
    </button>
    
    <div 
      v-if="isOpen" 
      class="dropdown-menu show position-absolute shadow-lg"
      :style="{ top: '100%', left: '0', zIndex: 1050 }"
    >
      <button 
        class="dropdown-item" 
        @click="handleDownloadPdf"
        type="button"
      >
        Descargar PDF
      </button>
      <button 
        class="dropdown-item" 
        @click="handleSendEmail"
        type="button"
      >
        Enviar PDF a mi correo
      </button>
    </div>
    
    <div 
      v-if="isOpen" 
      class="dropdown-overlay position-fixed"
      @click="closeDropdown"
      style="top: 0; left: 0; width: 100vw; height: 100vh; z-index: 1040;"
    ></div>
  </div>
</template>

<script>
import { generatePdfFromElement } from '@/utils/fileUtils';

export default {
  name: 'ExportDropdown',
  props: {
    elementId: {
      type: String,
      required: true,
      default: 'table-to-export'
    },
    filename: {
      type: String,
      default: 'document.pdf'
    },
    emailSubject: {
      type: String,
      default: 'Documento PDF'
    }
  },
  data() {
    return {
      isOpen: false,
    };
  },
  methods: {
    toggleDropdown() {
      this.isOpen = !this.isOpen;
    },
    closeDropdown() {
      this.isOpen = false;
    },
    async handleDownloadPdf() {
      try {
        const element = document.getElementById(this.elementId);
        if (!element) {
          console.error(`Element with ID '${this.elementId}' not found!`);
          return;
        }
        console.log("Element found:", element);
        const pdf = await generatePdfFromElement(element, this.filename);
        pdf.triggerUserDownload();
      } catch (error) {
        console.error("Error exporting to PDF:", error);
      } finally {
        this.closeDropdown();
      }
    },
    async handleSendEmail() {
      try {
        console.log("Sending email with PDF...");
        const element = document.getElementById(this.elementId);
        if (!element) {
          console.error(`Element with ID '${this.elementId}' not found!`);
          return;
        }
        const pdf = await generatePdfFromElement(element, this.filename);
        pdf.sendToCurrentUserEmail(this.emailSubject);
      } catch (error) {
        console.error("Error sending email with PDF:", error);
      } finally {
        this.closeDropdown();
      }
    },
    handleKeyDown(event) {
      if (event.key === 'Escape' && this.isOpen) {
        this.closeDropdown();
      }
    },
  },
  mounted() {
    document.addEventListener('keydown', this.handleKeyDown);
  },
  unmounted() {
    document.removeEventListener('keydown', this.handleKeyDown);
  },
};
</script>

<style scoped>
.export-dropdown {
  display: inline-block;
}

.dropdown-menu {
  min-width: 200px;
  border: 1px solid #dee2e6;
  border-radius: 0.375rem;
  background-color: #fff;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
}

.dropdown-item {
  padding: 0.5rem 1rem;
  border: none;
  background: none;
  width: 100%;
  text-align: left;
  color: #212529;
  text-decoration: none;
  transition: background-color 0.15s ease-in-out;
}

.dropdown-item:hover {
  background-color: #f8f9fa;
  color: #1e2125;
}

.dropdown-item:focus {
  background-color: #e9ecef;
  color: #1e2125;
  outline: none;
}

.dropdown-toggle::after {
  margin-left: 0.5rem;
}

.dropdown-overlay {
  background: transparent;
}
</style> 