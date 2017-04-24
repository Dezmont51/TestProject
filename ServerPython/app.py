#!flask/bin/python
# -*- coding: utf-8 -*-
from flask import Flask,jsonify,abort
from flask_httpauth import HTTPBasicAuth
from OpenSSL import SSL




auth=HTTPBasicAuth()
app=Flask(__name__)

@auth.get_password
def get_password(username):
 if username=='123':
  return '123'
 return None

@auth.error_handler
def unauthorized():
 return "Хватит Вводить всякую чушь"



tasks=[
 {
  'id':1,
  'title':u'Fuck'
 },
 {
  'id':2,
  'title':u'Ass'
 },
 {
  'id':3,
  'title':u'ерполдолдлодлодлодлодлBitch'
 }
]





if(__name__=='__main__'):
 app.debug=True
 app.run(host='0.0.0.0')
