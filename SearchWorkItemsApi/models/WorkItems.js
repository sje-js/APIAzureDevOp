// WorkItems.js

const mongoose = require('mongoose');
const Schema = mongoose.Schema;

// Define collection and schema for Business
let WorkItems = new Schema({
  _id: {
    type: Object
  },
  title: {
    type: String
  },
  state: {
    type: String
  },
  created_date: {
      type: Date
  },
  workitem_type: {
      type: String
    }
},{
    collection: 'workitems'
});

module.exports = mongoose.model('WorkItems', WorkItems);