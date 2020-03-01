<template>
  <div class="upload">
    <form id="fileUploadForm" enctype="multipart/form-data">
      <input type="file" id="trxUpload" name="file" :file="file" v-on:change="fileChanged" />
    </form>
    <button type="button" id="uploadFile" v-on:click="uploadFile">Upload</button>
    <table v-if="transactions">
      <thead>
        <tr>
          <th>Transaction Date</th>
          <th>Post Date</th>
          <th>Reference</th>
          <th>Description</th>
          <th>Amount</th>
          <th>Account Number</th>
          <th>Card Number</th>
          <th>Cardholder Name</th>
          <th>MCC</th>
          <th>MCC Description</th>
          <th>MCC Group</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in transactions" v-bind:key="item.reference">
          <td>{{formatDate(new Date(item.transactionDate))}}</td>
          <td>{{formatDate(new Date(item.postDate))}}</td>
          <td>{{item.reference}}</td>
          <td>{{item.description}}</td>
          <td>{{item.amount}}</td>
          <td>{{item.accountNumber}}</td>
          <td>{{item.cardNumber}}</td>
          <td>{{item.cardholderName}}</td>
          <td>{{item.mcc}}</td>
          <td>{{item.mccDescription}}</td>
          <td>{{item.mccGroup}}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator'

class Transaction {
    transactionDate: string
    postDate: string
    reference: string
    description: string
    amount: number
    accountNumber: string
    cardNumber: string
    cardholderName: string
    mcc: number
    mccDescription: string
    mccGroup: string
}

@Component
export default class FileUpload extends Vue {
  public file: File
  public transactions: Array<Transaction> = []

  formatDate (date: Date): string {
    const day = date.getDate()
    const monthIdx = date.getMonth()
    const year = date.getFullYear()
    return `${monthIdx}/${day}/${year}`
  }

  fileChanged (elFile: any): void {
    this.file = elFile.target.files[0]
  }

  uploadFile (): void {
    const uploadForm: HTMLFormElement | null = document.querySelector('#fileUploadForm')
    const formData = new FormData(uploadForm)
    let trx: Array<Transaction>
    fetch('https://localhost:44330/Finance/upload', {
      method: 'post',
      body: formData
    }).then((resp) => {
      return resp.json()
    }).then((data) => {
      this.transactions = data
    })
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped lang="scss">
h3 {
  margin: 40px 0 0;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
