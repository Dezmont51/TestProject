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

def get_connect():
 return pymysql.connect(host='89.223.31.42', user='root', passwd='123', db='TestBase', charset="utf8")

def get_cur_in_sql(query):
    sql = query
    db = get_connect()
    cur = db.cursor()
    cur.execute(sql)
    return cur

def get_in_base_city():
 cur=get_cur_in_sql( "SELECT * FROM offices")
 return [dict(id=row[0],city=row[1],text=row[2]) for row in cur.fetchall()]

def get_in_base_user_dates():
 cur = get_cur_in_sql("select * from users as u left join savings as s on u.id_user=s.id_user")
 return [dict(id=row[0],city=row[1],text=row[2]) for row in cur.fetchall()]

def get_in_base(query_id):
 cur = get_cur_in_sql("SELECT * FROM Users WHERE id="+str(query_id))
 return cur.fetchone()


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
