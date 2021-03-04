// workitems.route.js

const express = require('express');
const app = express();
const workitemsRoutes = express.Router();

// Require Business model in our routes module
let WorkItems = require('../models/WorkItems');

// Defined get data(index or listing) route
workitemsRoutes.route('/').get(function (req, res) {
    WorkItems.find(function (err, workitems){
    if(err){
      console.log(err);
    }
    else {
      res.json(workitems);
    }
  });
});

module.exports = workitemsRoutes;