<template>
  <div class="home">
    <FileUpload />
  </div>
</template>

<script>
// @ is an alias to /src
import FileUpload from '@/components/FileUpload.vue'

export default {
  name: 'Home',
  components: {
    FileUpload
  }
}
</script>
